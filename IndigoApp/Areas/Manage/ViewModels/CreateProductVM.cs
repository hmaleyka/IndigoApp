namespace IndigoApp.Areas.Manage.ViewModels
{
    public class CreateProductVM
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImgUrl { get; set; }

    }
}
