using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Models
{
    public class HomeViewModel
    {
        public About? About { get; set; }

        public List<Feature> Features { get; set; } = new();

        public List<WhyChooseUs> WhyChooseUs { get; set; } = new();

        public List<Process> Processes { get; set; } = new();

        public List<Testimonial> Testimonials { get; set; } = new();

        public List<Faq> Faqs { get; set; } = new();

        public List<OurFuture> OurFutures { get; set; } = new();

        public List<Award> Awards { get; set; } = new();

        public List<Car> Cars { get; set; } = new();

        public List<Brand> Brands { get; set; } = new();

        public List<CarModel> Models { get; set; } = new();

        public List<Category> Categories { get; set; } = new();
        public List<Branch> Branches { get; set; } = new();
    }

}