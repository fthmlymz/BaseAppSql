﻿using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Shared;

namespace Web.API.middleware
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public RateLimitMiddleware(
            RequestDelegate next,
            IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // First, we check if the requested endpoint has the rate limit attribute
            if (!context.HasRateLimitAttribute(out var decorator))
            {
                // If not, we ignore the request by releasing it
                await _next(context);
                return;
            }

            // If the request has a rate limit, we will fetch the customer's consumption data
            var consumptionData = await _cache.GetCustomerConsumptionDataFromContextAsync(context);
            if (consumptionData is not null)
            {
                // If the customer has consumption data, we will check if he has reached the rate limit
                if (consumptionData.HasConsumedAllRequests(decorator!.TimeWindowInSeconds, decorator!.MaxRequests))
                {
                    // Upon reaching the rate limit, we create a failed Result with a message
                    var result = Result<string>.Failure("Too many requests. Please try again later.");
                    await WriteResponse(context, result);
                    return;
                }

                // However, if the customer has not reached the limit, we will increase their consumption
                consumptionData.IncreaseRequests(decorator!.MaxRequests);
            }

            // Finally, let's update the customer's consumption data
            await _cache.SetCacheValueAsync(context.GetCustomerKey(), consumptionData);

            // And continue the request
            await _next(context);
        }

        private static async Task WriteResponse<T>(HttpContext context, Result<T> result)
        {
            context.Response.StatusCode = result.Succeeded ? StatusCodes.Status200OK : StatusCodes.Status429TooManyRequests;
            context.Response.ContentType = "application/json";

            var json = JsonConvert.SerializeObject(result);
            await context.Response.WriteAsync(json);
        }
    }


    /*public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDistributedCache _cache;

        public RateLimitMiddleware(
            RequestDelegate next,
            IDistributedCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // First, we check if the requested endpoint has the rate limit attribute
            if (!context.HasRateLimitAttribute(out var decorator))
            {
                // If not, we ignore the request by releasing it
                await _next(context);
                return;
            }

            // If the request has a rate limit, we will fetch the customer's consumption data
            var consumptionData = await _cache.GetCustomerConsumptionDataFromContextAsync(context);
            if (consumptionData is not null)
            {
                // If the customer has consumption data, we will check if he has reached the rate limit
                if (consumptionData.HasConsumedAllRequests(decorator!.TimeWindowInSeconds, decorator!.MaxRequests))
                {
                    // Upon reaching the rate limit, we return too many requests status
                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    return;
                }

                // However, if the customer has not reached the limit, we will increase their consumption
                consumptionData.IncreaseRequests(decorator!.MaxRequests);
            }

            // Finally, let's update the customer's consumption data
            await _cache.SetCacheValueAsync(context.GetCustomerKey(), consumptionData);

            // And continue the request
            await _next(context);
        }
    }*/
}