namespace eStoreOnline.Models;

public class ResponseModel<TModel>
{
    public bool IsSuccess { get; set; }

    public TModel? Data { get; set; }

    public string? Error { get; set; }

    public static ResponseModel<TModel> Success(TModel data)
    {
        return new ResponseModel<TModel>
        {
            IsSuccess = true,
            Data = data
        };
    }

    public static ResponseModel<TModel> Fail(string error)
    {
        return new ResponseModel<TModel>
        {
            IsSuccess = false,
            Error = error
        };
    }

    public static ResponseModel<TModel> Success()
    {
        return new ResponseModel<TModel>
        {
            IsSuccess = true
        };
    }

    public static ResponseModel<TModel> Fail()
    {
        return new ResponseModel<TModel>
        {
            IsSuccess = false
        };
    }
}

public class ResponseModel
{
    public bool IsSuccess { get; set; }

    public string? Error { get; set; }

    public static ResponseModel Success()
    {
        return new ResponseModel
        {
            IsSuccess = true
        };
    }

    public static ResponseModel Fail(string error)
    {
        return new ResponseModel
        {
            IsSuccess = false,
            Error = error
        };
    }

    public static ResponseModel Fail()
    {
        return new ResponseModel
        {
            IsSuccess = false
        };
    }
}