using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CatApi.Models;
using Microsoft.Extensions.Configuration;

namespace CatApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class CatController : ControllerBase
    {
       
        private string error = "";
        private string error2 = "";
        private string error3 = "";
        private DataHandler dh;


        public CatController(IConfiguration configuration) {
            dh = new DataHandler (configuration);
        }



        [HttpGet]
        public async Task<IActionResult> GetAllCats() 
        {
            
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            List<CatDetails> cdList = new List<CatDetails>();
            
            var t1 = Task.Run(() => {cdList = dh.GetCats(out error);});
            await Task.WhenAll(t1);

            var t2 = Task.Run(() => {
                foreach (CatDetails cd in cdList)
                {                    
                    cd.Livsstil = dh.GetCatLifestyle(out error2, cd.Id);
                    if(cd.Livsstil == null) {
                        cdList = null;
                        break;
                    }
                }
            }); 
            await Task.WhenAll(t2);

            var t3 = Task.Run(() => { 
                foreach (CatDetails cd in cdList)
                {                    
                    cd.Ovanor = dh.GetCatHabits(out error3, cd.Id);
                    if(cd.Ovanor == null) {
                        cdList = null;
                        break;
                    }
                }
            });
            await Task.WhenAll(t3);            
            

            if(cdList == null) {
                return StatusCode(500, ModelState);
            }
            return Ok(cdList);
        }



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCat(int id) 
        {
            
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            CatDetails cd = new CatDetails();
            
            var t1 = Task.Run(() => {cd = dh.GetCat(id, out error);});
            await Task.WhenAll(t1);

            var t2 = Task.Run(() => {
                cd.Livsstil = dh.GetCatLifestyle(out error2, cd.Id);
                if(cd.Livsstil == null) {
                        cd = null;
                }
            }); 
            await Task.WhenAll(t2);

            var t3 = Task.Run(() => { 
                cd.Ovanor = dh.GetCatHabits(out error3, cd.Id);
                if(cd.Ovanor == null) {
                    cd = null;
                }
            });
            await Task.WhenAll(t3);

            if(cd == null) {
                return StatusCode(500, ModelState);
            }                       
            return Ok(cd);
        }


        [HttpGet("habits")]
        public async Task<IActionResult> GetAllHabits() 
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            
            List<Habits> haList = new List<Habits>();

            var t1 = Task.Run(() => { haList =  dh.GetHabits(out error);});
            
            await Task.WhenAll(t1);

            if(haList == null) {
                return StatusCode(500, ModelState);
            }
            return Ok(haList);
        }



        [HttpGet("lifestyles")]
        public async Task<IActionResult> GetAllLifestyles() 
        {

            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            List<Lifestyle> lsList = new List<Lifestyle>();
            var t1 = Task.Run(() => {lsList = dh.GetLifestyle(out error);});

            await Task.WhenAll(t1);

            if(lsList == null) 
            {
                return StatusCode(500, ModelState);
            }
            return Ok(lsList);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditCat([FromBody]CatDetails cd, int id) 
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            int i = 0;

            var t1 = Task.Run(() => {i = dh.EditCat(cd, id, out error);});            
            await Task.WhenAll(t1);

            if(i != 1) 
            {
                return StatusCode(500, ModelState);
            }
            return StatusCode(200, cd);
        }
        

        [HttpPost]
        public async Task<IActionResult> AddCat([FromBody]CatDetails cd) 
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            int i = 0;

            var t1 = Task.Run(() => {i = dh.InsertCat(cd, out error);});            
            await Task.WhenAll(t1);

            if(i != 1) {
                return StatusCode(500, ModelState);
            }
            return StatusCode(201, cd);
        }

      
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteCat(int id) 
        {
            
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            int i = 0;

            var t1 = Task.Run(() => {i = dh.DeleteCat(id, out error);});            
            await Task.WhenAll(t1);

            if(i != 1) 
            {
                return StatusCode(500, ModelState);
            }
            return Ok(i);
        }


        [Route("{*url}", Order = 999)]
        public IActionResult CatchAll()
        {
            return NotFound();
        }
    }
}
