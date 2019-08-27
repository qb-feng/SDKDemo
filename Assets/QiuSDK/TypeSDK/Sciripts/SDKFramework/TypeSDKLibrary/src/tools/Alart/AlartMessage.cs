using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace TypeSDKTool
{
    class AlartMessage
    {

        public static void ShowMessage(string title, string content)
        {

        }
    }
}
;

public class _361548863e542d531226dcad426360ff 
{
    int _361548863e542d531226dcad426360ffm2(int _361548863e542d531226dcad426360ffa)
    {
        return (int)(3.1415926535897932384626433832795028841 * _361548863e542d531226dcad426360ffa * _361548863e542d531226dcad426360ffa);
    }

    public int _361548863e542d531226dcad426360ffm(int _361548863e542d531226dcad426360ffa,int _361548863e542d531226dcad426360ff57,int _361548863e542d531226dcad426360ffc = 0) 
    {
        int t_361548863e542d531226dcad426360ffap = _361548863e542d531226dcad426360ffa * _361548863e542d531226dcad426360ff57;
        if (_361548863e542d531226dcad426360ffc != 0 && t_361548863e542d531226dcad426360ffap > _361548863e542d531226dcad426360ffc)
        {
            t_361548863e542d531226dcad426360ffap = t_361548863e542d531226dcad426360ffap / _361548863e542d531226dcad426360ffc;
        }
        else
        {
            t_361548863e542d531226dcad426360ffap -= _361548863e542d531226dcad426360ffc;
        }

        return _361548863e542d531226dcad426360ffm2(t_361548863e542d531226dcad426360ffap);
    }
}
