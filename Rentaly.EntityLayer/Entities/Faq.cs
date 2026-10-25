namespace Rentaly.EntityLayer.Entities
{
    public class Faq
    {
        public int FaqId { get; set; }

        public string Question { get; set; }

        public string Answer { get; set; }

        public int Order { get; set; }

        public bool Status { get; set; }
    }
}