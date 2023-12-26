using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.ViewComponents
{//her sayfada etiketleri göstermek için tag kodlarını yazmak lazım bu şekilde çalışmak istemedim bunun yerine component oluşturup etiketleri mesela orada oluşturup istediğim sayfada ordan direkt çektim,parçalı view de onları ayrı ayrı tanımlamak lazım ama burda direk ordan alabildim
//oluşturduğum component yapısını view içine entegre ettim
    public class TagsMenu: ViewComponent
    {
        private ITagRepository _tagsRepository;
        public TagsMenu(ITagRepository tagsRepository){
            _tagsRepository = tagsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(){
            return View(await _tagsRepository.Tags.ToListAsync());
        }
    }
}