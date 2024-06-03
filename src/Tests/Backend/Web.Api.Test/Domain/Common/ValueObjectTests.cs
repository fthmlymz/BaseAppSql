using Domain.Common;

namespace Web.Api.Test.Domain.Common
{
    [TestFixture]
    public class ValueObjectTests
    {
        private class TestValueObject : ValueObject
        {
            public string Property1 { get; set; }
            public int Property2 { get; set; }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                yield return Property1;
                yield return Property2;
            }
        }

        [Test]
        public void Equals_TwoEqualValueObjects_ReturnsTrue()
        {
            // Arrange
            var obj1 = new TestValueObject { Property1 = "Test", Property2 = 123 };
            var obj2 = new TestValueObject { Property1 = "Test", Property2 = 123 };

            // Act
            var result = obj1.Equals(obj2);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void Equals_TwoDifferentValueObjects_ReturnsFalse()
        {
            // Arrange
            var obj1 = new TestValueObject { Property1 = "Test1", Property2 = 123 };
            var obj2 = new TestValueObject { Property1 = "Test2", Property2 = 123 };

            // Act
            var result = obj1.Equals(obj2);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetHashCode_TwoEqualValueObjects_ReturnsSameHashCode()
        {
            // Arrange
            var obj1 = new TestValueObject { Property1 = "Test", Property2 = 123 };
            var obj2 = new TestValueObject { Property1 = "Test", Property2 = 123 };

            // Act
            var hashCode1 = obj1.GetHashCode();
            var hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.AreEqual(hashCode1, hashCode2);
            Assert.That(hashCode2, Is.EqualTo(hashCode1));
        }

        [Test]
        public void GetHashCode_TwoDifferentValueObjects_ReturnsDifferentHashCodes()
        {
            // Arrange
            var obj1 = new TestValueObject { Property1 = "Test1", Property2 = 123 };
            var obj2 = new TestValueObject { Property1 = "Test2", Property2 = 123 };

            // Act
            var hashCode1 = obj1.GetHashCode();
            var hashCode2 = obj2.GetHashCode();

            // Assert
            Assert.AreNotEqual(hashCode1, hashCode2);
            Assert.That(hashCode2, Is.Not.EqualTo(hashCode1));
        }
    }
}
