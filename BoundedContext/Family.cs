using Newtonsoft.Json;

namespace BoundedContext
{
    public class Family : IAggregateRoot
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string LastName { get; set; }
        public Parent[] Parents { get; set; }
        public Child[] Children { get; set; }
        public Address Address { get; set; }
        public bool IsRegistered { get; set; }
        public string PartitionKey => LastName;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


}
