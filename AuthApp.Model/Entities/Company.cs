namespace AuthApp.Model.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserCompany> UserCompanies { get; set; }
    }
}
