namespace Rentaly.EntityLayer.Entities
{
    public class Award
    {
        public int AwardId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public int Order { get; set; }

        public bool Status { get; set; }
    }
}