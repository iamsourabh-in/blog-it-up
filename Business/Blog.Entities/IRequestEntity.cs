using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Entities
{
    public interface IRequestEntity<out TResponse> : IBaseEntity
    {
    }
}
