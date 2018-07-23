using System.ComponentModel.DataAnnotations;

namespace Seqrus.Web.ViewModels
{
    public class SprocketCartModel
    {
        [Range(1, 9999999)]
        public int Quantity { get; set; }

        public string Address { get; set; }
    }
}