//using App.Data;
//using App.Models;

//namespace App.Logic
//{
//    public class TillAccountLogic
//    {

//        private readonly AppDbContext _context;

//        public TillAccountLogic(AppDbContext context)
//        {
//            _context = context;
//        }

//        public List<TillUser> GetAll()
//        {
//            return _context.TillUser.ToList();
//        }


//        public List<ApplicationUser> GetAllTellers()
//        {
//            return _context.Users.ToList(); //.Where(u => u.Role.RoleClaims.Any(r => r.Name.Equals("TellerPosting"))).ToList();
//        }


//        public List<ApplicationUser> TellersWithoutTill()
//        {
//            var tellers = GetAllTellers();
//            var output = new List<ApplicationUser>();

//            var tillToUsers = _context.TillUser.ToList();
//            foreach (var teller in tellers)
//            {
//                if (!tillToUsers.Any(tu => tu.UserId == teller.Id))
//                {
//                    output.Add(teller);
//                }
//            }
//            return output;
//        }

//        public List<GLAccount> GetAllTills()
//        {
//            return _context.GLAccount.Where(a => a.AccountName.ToLower().Contains("till") && a.GLCategory.MainCategory == MainGLCategory.Asset).ToList();
//        }

//        public List<GLAccount> TillsWithoutTeller()
//        {
//            var tills = GetAllTills();

//            var output = new List<GLAccount>();
//            var tillToUsers = _context.TillUser.ToList();

//            foreach (var till in tills)
//            {
//                if (!tillToUsers.Any(tu => tu.GLAccountID == till.AccountID))
//                {
//                    output.Add(till);
//                }
//            }
//            return output;
//        }





//        public List<TillUser> ExtractAllTellerInfo()
//        {
//            var output = new List<TillUser>();
//            var tellersWithTill = GetAll();
//            var tellersWithoutTill = TellersWithoutTill();

//            foreach (var teller in tellersWithoutTill)
//            {
//                output.Add(new TillUser { UserId = teller.Id, GLAccountID = 0 });
//            }

//            output.AddRange(tellersWithTill);
//            return output;

//        }

//        public List<ApplicationUser> ExtractTellersWithoutTill()
//        {
//            return TellersWithoutTill();
//        }

//        public List<GLAccount> ExtractTillsWithoutTeller()
//        {
//            return TillsWithoutTeller();
//        }
//    }
//}
