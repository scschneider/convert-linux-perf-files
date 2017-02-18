class TypeConversionHelper
{
    public bool ConvertTypeToBool(string typeValue)
    {
        bool pv;
        switch (typeValue)
        {
            case "1":
            case "true":
            case "yes":
                pv = true;
                break;
            case "0":
            case "false":
            case "no":
                pv = false;
                break;
            default:
                pv = false;
                break;
        }
        
        return pv;
    }
}