using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.BusinessLayer.ValidationRules
{
    public class CategoryValidator:AbstractValidator<Category>
    {

        public CategoryValidator()
        {
            RuleFor(x => x.CategoryName).NotEmpty().WithMessage("Kategori Adı Boş Geçilemez");
            RuleFor(x => x.CategoryName).MinimumLength(2).WithMessage("Kategori adı en az 2 karakter olmalıdır").MinimumLength(30).WithMessage("Kategori adı en fazla 30 karakter olmalıdır!");
        
        }
    }
}
