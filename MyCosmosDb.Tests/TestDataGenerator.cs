using System.Runtime.InteropServices;

namespace MySampleCosmosDb.Tests
{
    public class TestDataGenerator
    {
        private int counter = 0;
        
        public Family CreateRandomFamily(string familyLastName)
        {
            counter += 1;
            Family andersenFamily = new Family
            {
                Id = $"{familyLastName}.{counter}",
                LastName = familyLastName,
                Parents = new Parent[]
                {
                    new Parent { FirstName = "Thomas" },
                    new Parent { FirstName = "Mary Kay" }
                },
                Children = new Child[]
                {
                    new Child
                    {
                        FirstName = "Henriette Thaulow",
                        Gender = "female",
                        Grade = 5,
                        Pets = new Pet[]
                        {
                            new Pet { GivenName = "Fluffy" }
                        }
                    }
                },
                Address = new Address { State = "WA", County = "King", City = "Seattle" },
                IsRegistered = false
            };
            
            return andersenFamily;
        }

      
    }
}
