using System.Runtime.Serialization;

namespace FoodOrderGuiClient.Models
{
    [DataContract]
    public class RestaurantItem
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string? Name { get; set; }

        [DataMember(Name = "menu")]
        public string[]? Menu { get; set; }
    }
}