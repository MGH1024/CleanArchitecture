using Domain.Base;
using Domain.Enums;

namespace Domain.Entities.Public;

public class State : AuditableEntity, IPageable, ICodeable, IDropdownable
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }


    //codable
    public int Code { get; set; } = 1;

    public States CodeEnum
    {
        get
        {
            var res = ((States) Code);
            return res;
        }
    }

    //pageable
    public int Row { get; set; }

    public int TotalCount { get; set; }

    public int CurrentPage { get; set; }

    public int PageSize { get; set; }


    //dropdownable
    public string ListItemText
    {
        get
        {
            var strResult = Title + "-" + Code;
            return strResult;
        }
    }

    public string ListItemTextForAdmins
    {
        get
        {
            var strResult = Title + "-" + Code + "_" + IsActive.ToString();
            return strResult;
        }
    }
}