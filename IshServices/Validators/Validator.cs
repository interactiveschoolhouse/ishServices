using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IshServices.Validators
{
    public interface Validator<T>
    {
        ValidatorResult Validate(T target);
    }
}
