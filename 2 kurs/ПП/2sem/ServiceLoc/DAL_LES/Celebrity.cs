namespace DAL_LES
{
    public interface ICelebrity : ICelebrity<Celebrity> { }
    public class Celebrity
    {
        public Celebrity() { this.FullName = ""; this.Nationality = "XX"; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Nationality { get; set; }
        public string? ReqPhotoPath { get; set; }
    }

}
