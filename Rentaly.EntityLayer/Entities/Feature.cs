namespace Rentaly.EntityLayer.Entities
{
    public class Feature
    {
        public int FeatureId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public int Order { get; set; }

        public bool Status { get; set; }
    }
}