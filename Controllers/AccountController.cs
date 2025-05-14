using System.Linq;
using System.Web.Mvc;
using System.Web.Security; // ✅ Add this
using Event4U.Models;

namespace Event4U.Controllers
{
    public class AccountController : Controller
    {
        Event4UContext db = new Event4UContext();

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

       [HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Login(string email, string password)
{
            // ✅ Check for hardcoded admin credentials
            if (email == "admin@gmail.com" && password == "admin123")
            {
                FormsAuthentication.SetAuthCookie("admin@gmail.com", false);
                Session["UserName"] = "Admin";           // 👈 this is what will be displayed
                Session["IsAdmin"] = true;

                return RedirectToAction("Index", "Admin");
            }


            // 🔍 Regular user authentication from DB
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
    if (user != null)
    {
        // Set authentication and session
        FormsAuthentication.SetAuthCookie(user.Email, false);
        Session["UserId"] = user.UserId;
        Session["UserName"] = user.Name;
        Session["IsAdmin"] = false;

        return RedirectToAction("Index", "Home");
    }

    ViewBag.Message = "Invalid credentials!";
    return View();
}
 public ActionResult Logout()
        {
            // ✅ Clear cookie + session
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
