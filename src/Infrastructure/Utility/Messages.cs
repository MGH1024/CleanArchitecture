namespace Utility;

public static class Messages
{
    public static string SuccessOperation => "عملیات با موفقیت به پایان رسید";

    public static string RecordNotFound => "رکورد مورد نظر شما یافت نشد";

    public static string TokenNotFound => "توکن اهراز هویت یافت نشد";

    public static string TokenExpire => "توکن اهراز هویت منقضی شده است";

    public static string ForeignKeyException => "به علت استفاده در بخش دیگر قابل حذف نمی باشد";

    public static string UnknownException => "موقتا مشکلی در سیستم پیش آمده است لطفا دقایقی دیگر تلاش نمایید";

    public static string BadDataFromClient => "اطلاعات ارسال شده معتبر نمیباشد";

    public static string DuplicateInsert => "رکورد مورد نظر قبلا درج شده است";

    public static string InvalidField => "مقدار نا معتبر است";

    public static string ListDataInvalid => "داده های لیست معتبر نمی باشد";

    public static string CreateState => "برای ساخت استیت";

    public static string ErrorInCallExternalApiService => "در فراخوانی سرویس خارجی مشکلی به وجود امده است" +
        ".لطفا چند دقیقه دیگر دوباره تلاش کنید";

    //validation
    public static string NotNull => "نباید نال باشد";
}
