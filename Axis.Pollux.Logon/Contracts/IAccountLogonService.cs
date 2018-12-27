using System;
using Axis.Luna.Operation;
using Axis.Pollux.Common.Utils;
using Axis.Pollux.Logon.Contracts.Params;
using Axis.Pollux.Logon.Models;

namespace Axis.Pollux.Logon.Contracts
{
    public interface IAccountLogonService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="publicCredential"></param>
        /// <param name="secretCredential"></param>
        /// <returns></returns>
        Operation<LogonSession> Login(LogonCredential publicCredential, LogonCredential secretCredential);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Operation<UserLogon> GetUserLogonWithToken(Guid userId, string token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="ipaddress"></param>
        /// <param name="locale"></param>
        /// <param name="device"></param>
        /// <param name="sliceQuery"></param>
        /// <returns></returns>
        Operation<ArrayPage<UserLogon>> GetUserLogons(
            Guid userId,
            string ipaddress = null,
            string locale = null,
            string device = null,
            ArrayPageRequest sliceQuery = null);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ipaddress"></param>
        /// <param name="locale"></param>
        /// <param name="device"></param>
        /// <param name="sliceQuery"></param>
        /// <returns></returns>
        Operation<ArrayPage<UserLogon>> GetSystemLogons(
            string ipaddress = null,
            string locale = null,
            string device = null,
            ArrayPageRequest sliceQuery = null);
    }
}
