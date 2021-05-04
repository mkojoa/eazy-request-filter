using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eazy.request.filter.Filter.Options
{
    [Flags]
    public enum Verification
    {
        Company,
        User
    }

    [Flags]
    public enum ErrorType
    {
        Company,
        User
    }

    [Flags]
    public enum RoleGate
    {
        Administrator,
        SuperAdmin
    }

    public static class VerificationTextExtender
    {
        public static string AsText(this Verification operation)
        {
            var switchResult = operation switch
            {
                Verification.Company => "Company",
                Verification.User => "User",
                _ => throw new NotImplementedException(),
            };

            return switchResult;
        }
    }

    public static class ErrorTypeTextExtender
    {
        public static string AsText(this ErrorType operation)
        {
            var switchResult = operation switch
            {
                ErrorType.Company => "",
                ErrorType.User => "",
                _ => throw new NotImplementedException(),
            };

            return switchResult;
        }
    }

    public static class RoleGateTextExtender
    {
        public static string AsText(this RoleGate operation)
        {
            var switchResult = operation switch
            {
                RoleGate.Administrator => "",
                RoleGate.SuperAdmin => "",
                _ => throw new NotImplementedException(),
            };

            return switchResult;
        }
    }

}
