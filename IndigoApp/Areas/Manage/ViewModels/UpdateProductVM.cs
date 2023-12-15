namespace IndigoApp.Areas.Manage.ViewModels
{
    public class UpdateProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImgUrl { get; set; }
        public string Image { get; set; }

    }
}
