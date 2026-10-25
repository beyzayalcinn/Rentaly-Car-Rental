namespace Rentaly.EntityLayer.Entities
{
    public class Testimonial
    {
        public int TestimonialId { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public string ImageUrl { get; set; }

        public string Job { get; set; }

        public int Rating { get; set; }

        public bool Status { get; set; }
        public bool IsApproved { get; set; }
    }
}