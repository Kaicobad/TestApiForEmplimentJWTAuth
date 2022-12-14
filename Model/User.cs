namespace testapi.Model
{
    public class User
    {
        [key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }
        public string Passwword { get; set; }
    }
}
