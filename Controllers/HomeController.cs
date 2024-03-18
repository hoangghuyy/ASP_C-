using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Newtonsoft.Json;
using System.Diagnostics;
using Web_Asm2.Data;
using Web_Asm2.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Web_Asm2.Controllers
{
	public class HomeController : Controller
	{
		private readonly Web_Asm2Context _context;

		public HomeController(Web_Asm2Context context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var webApplication10Context = _context.Product.Include(p => p.Category);
			return View(await webApplication10Context.ToListAsync());
		}
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Product
				.Include(p => p.Category)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		[HttpPost]
		public async Task<IActionResult> CartCart(CartModel model)
		{
			var cart = HttpContext.Session.GetString("cart");

			if (cart == null)
			{
				var product = _context.Product.Find(model.Id);

				if (product != null)
				{
					List<Cart> listCart = new List<Cart>()
			{
				new Cart
				{
					Product = product,
					Quantity = model.Quantity
				}
			};

					HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(listCart));
				}
			}
			else
			{
				List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

				if (dataCart == null)
				{
					dataCart = new List<Cart>
			{
				new Cart
				{
					Product = _context.Product.Find(model.Id),
					Quantity = model.Quantity
				}
			};
				}
				else
				{
					bool check = true;

					for (int i = 0; i < dataCart.Count; i++)
					{
						if (dataCart[i].Product != null && dataCart[i].Product.Id == model.Id)
						{
							dataCart[i].Quantity++;
							check = false;
						}
					}

					if (check)
					{
						var product = _context.Product.Find(model.Id);

						if (product != null)
						{
							dataCart.Add(new Cart
							{
								Product = product,
								Quantity = model.Quantity
							});
						}
					}
				}

				HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
			}

			return RedirectToAction("CartShow");
		}


		public IActionResult CartShow()
		{

			var cart = HttpContext.Session.GetString("cart");
			if (cart != null)
			{
				List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

				if (dataCart.Count > 0)
				{
					ViewBag.carts = dataCart;
					return View();
				}
				return RedirectToAction(nameof(CartShow));
			}
			return RedirectToAction(nameof(CartShow));
		}

		public IActionResult UpdateCart(CartModel model)
		{
			var cart = HttpContext.Session.GetString("cart");
			if (cart != null)
			{
				List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);
				if (model.Quantity > 0)
				{
					for (int i = 0; i < dataCart.Count; i++)
					{
						if (dataCart[i].Product.Id == model.Id)
						{
							dataCart[i].Quantity = model.Quantity;
						}
					}
					HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));
				}
				return RedirectToAction(nameof(CartShow));
			}
			return BadRequest();

		}

		public async Task<IActionResult> DeleteCart(int id)
		{
			var cart = HttpContext.Session.GetString("cart");
			if (cart != null)
			{
				List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

				for (int i = 0; i < dataCart.Count; i++)
				{
					if (dataCart[i].Product.Id == id)
					{
						dataCart.RemoveAt(i);
					}
				}
				HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(dataCart));

				return RedirectToAction(nameof(CartShow));
			}
			return RedirectToAction(nameof(CartShow));
		}

		public async Task<IActionResult> ProductOrder(User user)
		{
			var cart = HttpContext.Session.GetString("cart");

			if (cart != null)
			{
				List<Cart> dataCart = JsonConvert.DeserializeObject<List<Cart>>(cart);

				foreach (var cartItem in dataCart)
				{
					Order newOrder = new Order
					{
						Date = DateTime.Now,
						TotalPrice = cartItem.Product.Price * cartItem.Quantity,
						Email = user.Email,
						Phone = user.Phone,
						Name = user.Name,
						Address = user.Address,
						ProductId = cartItem.Product.Id
					};

					_context.Order.Add(newOrder);
				}
				
				await _context.SaveChangesAsync();
				HttpContext.Session.Remove("cart");
				HttpContext.Session.SetString("email", user.Email);
				
				return RedirectToAction(nameof(ShowOrder));
			}

			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> ShowOrder()
		{
			var email = HttpContext.Session.GetString("email");
			var ordersWithProducts = await _context.Order
	        .Join(
		       _context.Product,
		       order => order.ProductId,
		       product => product.Id,
				(order, product) => new
				{
					Order = order,
					Product = product
				}
			)
			.Where(op => op.Order.Email == email)
			.ToListAsync();
			ViewBag.OrdersWithProducts = ordersWithProducts;
			return View();
		}


		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
