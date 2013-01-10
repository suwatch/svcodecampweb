/*
Copyright © 2005, Peter Kellner
All rights reserved.
http://peterkellner.net

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions
are met:

- Redistributions of source code must retain the above copyright
notice, this list of conditions and the following disclaimer.

- Neither Peter Kellner, nor the names of its
contributors may be used to endorse or promote products
derived from this software without specific prior written 
permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT,
INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES INCLUDING,
BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
POSSIBILITY OF SUCH DAMAGE.
*/


using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Collections.ObjectModel;

namespace MembershipUtilities
{

    /// <summary>
    /// Class Used to Select, Update, Insert and Delete
    /// From the Microsoft ASP.NET Membership API
    /// </summary>
    /// 
    [DataObject(true)]  // This attribute allows the ObjectDataSource wizard to see this class
    public class MembershipUserODS
    {

        /// <summary>
        /// This insert method is the default insert method.  It is typically associated with
        /// a detailview control for inserting new members.
        /// </summary>
        /// <param name="userName">MembershipUser.UserName</param>
        /// <param name="password">MembershipUser.password</param>
        /// <param name="isApproved">MembershipUser.IsApproved</param>
        /// <param name="comment">MembershipUser.comment</param>
        /// <param name="lastLockoutDate">MembershipUser.lastLockoutDate</param>
        /// <param name="creationDate">MembershipUser.creationDate</param>
        /// <param name="email">MembershipUser.email</param>
        /// <param name="lastActivityDate">MembershipUser.lastActivityDate</param>
        /// <param name="providerName">MembershipUser.providerName</param>
        /// <param name="isLockedOut">MembershipUser.isLockedOut</param>
        /// <param name="lastLoginDate">MembershipUser.lastLoginDate</param>
        /// <param name="isOnline">MembershipUser.isOnline</param>
        /// <param name="passwordQuestion">MembershipUser.passwordQuestion</param>
        /// <param name="lastPasswordChangedDate">MembershipUser.lastPasswordChangedDate</param>
        /// 
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        static public void Insert(string userName, bool isApproved,
            string comment, DateTime lastLockoutDate, DateTime creationDate,
            string email, DateTime lastActivityDate, string providerName, bool isLockedOut,
            DateTime lastLoginDate, bool isOnline, string passwordQuestion,
            DateTime lastPasswordChangedDate, string password, string passwordAnswer)
        {


            // The incoming parameters, password and passwordAnswer are not properties of the
            // MembershipUser class.  Membership has special member functions to deal with these
            // two special properties for security reasons.  For this reason, they do not appear
            // in a datacontrol that is created with this user object.  
            //
            // the only reason you may want to have defaults is so you can build insert into your
            // datacontrol.  A better approach would be to either follow the example shown in the
            // Membership.asp page where the parameters are set directly to the userobject, or not
            // include "new" at all in your control and use the other controls in the Membership API
            // for creating new members.  (CreateUserWizard, etc)
            //
            // It is recommended that you only enable the following lines if you are sure of what you are doing

            //if (password == null)
            //{
            //    password = "pass0word";
            //}

            //if (passwordAnswer == null)
            //{
            //    passwordAnswer = "Password Answer";
            //}



            MembershipCreateStatus status;
            Membership.CreateUser(userName, password, email, passwordQuestion, passwordAnswer, isApproved, out status);

            if (status != MembershipCreateStatus.Success)
            {
                throw new ApplicationException(status.ToString());
            }

            MembershipUser mu = Membership.GetUser(userName);
            mu.Comment = comment;
            Membership.UpdateUser(mu);

        }

        /// <summary>
        /// Takes as input the original username and the current username
        /// </summary>
        /// <param name="providerUserKey"></param>
        /// <param name="Original_providerUserKey"></param>
        /// 
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        static public void Delete(string UserName)
        {
            Membership.DeleteUser(UserName, true);
        }

        /// <summary>
        /// This update method is the default update as shown by the class attribute.
        /// 
        /// </summary>
        /// <param name="original_UserName">MembershipUser.UserName originally retrieved</param>
        /// <param name="email">MembershipUser.email</param>
        /// <param name="isApproved">MembershipUser.isApproved</param>
        /// <param name="comment">MembershipUser.comment</param>
        /// <param name="password">MembershipUser.password</param>
        /// <param name="passwordAnswer">MembershipUser.passwordAnswer</param>
        /// <param name="lastActivityDate">MembershipUser.lastActivityDate</param>
        /// <param name="lastLoginDate">MembershipUser.lastLoginDate</param>
        /// 
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        static public void Update(string UserName, string email,
             bool isApproved, string comment, DateTime lastActivityDate, DateTime lastLoginDate)
        {
            bool dirtyFlag = false;

            MembershipUser mu = Membership.GetUser(UserName);

            if ( string.IsNullOrEmpty(mu.Comment) || mu.Comment.CompareTo(comment) != 0)
            {
                dirtyFlag = true;
                mu.Comment = comment;
            }

            if (string.IsNullOrEmpty(mu.Email) || mu.Email.CompareTo(email) != 0)
            {
                dirtyFlag = true;
                mu.Email = email;
            }

            if (mu.IsApproved != isApproved)
            {
                dirtyFlag = true;
                mu.IsApproved = isApproved;
            }

            if (dirtyFlag == true)
            {
                Membership.UpdateUser(mu);
            }
        }

        /// <summary>
        /// This is just used to set the IsApproved status.
        /// username is always passed in for searching purposes.
        /// </summary>
        /// <param name="original_username">Current UserName to Update (primary key)</param>
        /// <param name="isApproved">MembershipUser.isApproved</param>
        /// 
        [DataObjectMethod(DataObjectMethodType.Update, false)]
        static public void Update(string Username, bool isApproved)
        {
            bool dirtyFlag = false;
            MembershipUser mu = Membership.GetUser(Username);

            if (mu.IsApproved != isApproved)
            {
                dirtyFlag = true;
                mu.IsApproved = isApproved;
            }
            if (dirtyFlag == true)
            {
                Membership.UpdateUser(mu);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        static public void Update(string UserName, string email,
             bool isApproved, string comment, DateTime lastActivityDate, DateTime lastLoginDate, string password)
        {
            bool dirtyFlag = false;

//Membership.Providers["AspNetSqlMembershipProviderNoQuestionAnswer"].GetUser(
            
            MembershipUser mu =
                Membership.GetUser(UserName, false);

            // http://forums.asp.net/1206792/ShowPost.aspx (how to have multiple providers)
            if (!string.IsNullOrEmpty(password))
            {
                string newPassword = mu.ResetPassword();
                mu.ChangePassword(newPassword, password);
            }


            if (mu.Comment == null || mu.Comment.CompareTo(comment) != 0)
            {
                dirtyFlag = true;
                mu.Comment = comment;
            }

            if (mu.Email == null || mu.Email.CompareTo(email) != 0)
            {
                dirtyFlag = true;
                mu.Email = email;
            }

            if (mu.IsApproved != isApproved)
            {
                dirtyFlag = true;
                mu.IsApproved = isApproved;
            }

            if (dirtyFlag == true)
            {
                Membership.UpdateUser(mu);
            }


        }

        [DataObjectMethod(DataObjectMethodType.Update, true)]
        static public void Update(string UserName, string email,
             bool isApproved,string password)
        {
            bool dirtyFlag = false;

            //Membership.Providers["AspNetSqlMembershipProviderNoQuestionAnswer"].GetUser(

            MembershipUser mu =
                Membership.GetUser(UserName, false);

            // http://forums.asp.net/1206792/ShowPost.aspx (how to have multiple providers)
            if (!string.IsNullOrEmpty(password))
            {
                string newPassword = mu.ResetPassword();
                mu.ChangePassword(newPassword, password);
            }


            if (mu.Email == null || mu.Email.CompareTo(email) != 0)
            {
                dirtyFlag = true;
                mu.Email = email;
            }

            if (mu.IsApproved != isApproved)
            {
                dirtyFlag = true;
                mu.IsApproved = isApproved;
            }

            if (dirtyFlag)
            {
                Membership.UpdateUser(mu);
            }


        }


        /// <summary>
        /// Make a list of MembershipUserWrapper objects
        /// </summary>
        /// <returns>A List of type MembershipUserWrapper</returns>
        /// 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        static public List<MembershipUserWrapper> GetMembers()
        {
            return GetMembers(true, true, null, null, -1, -1);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        static public List<MembershipUserWrapper> GetMembersByUsername(string userName)
        {
            return GetMembers(true, true, userName,null, -1, -1);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        static public List<MembershipUserWrapper> GetMembersByUsername(string userName,string SortData)
        {
            return GetMembers(true, true, userName, null, -1, -1);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        static public List<MembershipUserWrapper> GetMembers(string sortData,
            int maximumRows, int startRowIndex)
        {
            // All Users, All approvalStatus
            return GetMembers(true, true, null, sortData, maximumRows, startRowIndex);
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        static public List<MembershipUserWrapper> 
            GetMembersxxx(int maximumRows, int startRowIndex)
        {
            // All Users, All approvalStatus
            return GetMembers(true, true, null, string.Empty, -1,-1);
        }

        /// <summary>
        /// Make a list of MembershipUserWrapper objects by current sort
        /// </summary>
        /// <param name="sortData">Whicfh Column to perform the sort on</param>
        /// <returns>A List of type MembershipUserWrapper</returns>
        /// 
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        static public List<MembershipUserWrapper> GetMembers(string sortData)
        {
            // All Users, All approvalStatus
            return GetMembers(true, true, null, sortData,-1,-1);
        }

        /// <summary>
        /// returns all approved users by specified sort
        /// </summary>
        /// <param name="approvalStatus">if true, return approved users</param>
        /// <param name="sortData">description of sort</param>
        /// <returns>A List of type MembershipUserWrapper</returns>
        /// 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        static public List<MembershipUserWrapper> GetMembers(bool approvalStatus, string sortData)
        {
            if (approvalStatus == true)
            {
                return GetMembers(true, false, null, sortData,-1,-1);
            }
            else
            {
                return GetMembers(false, true, null, sortData, -1, -1);
            }
        }

        /// <summary>
        /// Return a collection of MembershipUserWrapper's based on criteria passed in as parameters
        /// </summary>
        /// <param name="returnAllApprovedUsers">returns all users with approval set to true</param>
        /// <param name="returnAllNotApproviedUsers">return all users with approval set to false</param>
        /// <param name="usernameToFind">return based on username (overrides approval above)</param>
        /// <param name="sortData">sort parameter</param>
        /// <returns>Returns a Collection of Users (as recommended by FxCop)</returns>
        /// 
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        static public List<MembershipUserWrapper> GetMembers(bool returnAllApprovedUsers, bool returnAllNotApprovedUsers,
            string usernameToFind, string sortData, int maximumRows, int startRowIndex)
        {

            List<MembershipUserWrapper> memberList = new List<MembershipUserWrapper>();

            // See if we are looking for just one user
            if (usernameToFind != null)
            {
                MembershipUser mu = Membership.GetUser(usernameToFind);
                if (mu != null)
                {
                    MembershipUserWrapper md = new MembershipUserWrapper(mu);
                    memberList.Add(md);
                }
            }
            else
            {
                MembershipUserCollection muc = null;
                if (startRowIndex == -1)
                {
                    muc = Membership.GetAllUsers();
                }
                else
                {
                    // We are given: startRowIndex and maximumRows to display
                    // We need to convert that to pageIndex,pageSize,totalRecords
                    //
                    // Assume, maximumRows is pageSize
                    // Need to figure out how many pageSizes before startRowIndex
                    // so
                    // 
                    int pageStart = startRowIndex / maximumRows;

                    // this is bad because it probably forces a retrieve of all records
                    // (what we are trying to avoid)
                    //int totalRecords = Membership.GetAllUsers().Count;
                    int totalRecords;
                    muc = Membership.GetAllUsers(pageStart, 5, out totalRecords);
                }
                foreach (MembershipUser mu in muc)
                {
                    if ((returnAllApprovedUsers == true && mu.IsApproved == true) ||
                         (returnAllNotApprovedUsers == true && mu.IsApproved == false))
                    {
                        MembershipUserWrapper md = new MembershipUserWrapper(mu);
                        memberList.Add(md);
                    }
                }

                if (sortData == null)
                {
                    sortData = "UserName";
                }
                if (sortData.Length == 0)
                {
                    sortData = "UserName";
                }

                // Make a special version of sortData for the switch statement so that whether or not the
                // DESC is appended to the string sortData, it will switch on the base of that string.
                string sortDataBase = sortData; // init and assume there is not DESC appended to sortData
                string descString = " DESC";
                if (sortData.EndsWith(descString))
                {
                    sortDataBase = sortData.Substring(0, sortData.Length - descString.Length);
                }

                Comparison<MembershipUserWrapper> comparison = null;

                switch (sortDataBase)
                {
                    case "UserName":
                        comparison = new Comparison<MembershipUserWrapper>(
                            delegate(MembershipUserWrapper lhs, MembershipUserWrapper rhs)
                            {
                                return lhs.UserName.CompareTo(rhs.UserName);
                            }
                            );
                        break;
                    case "Email":
                        comparison = new Comparison<MembershipUserWrapper>(
                             delegate(MembershipUserWrapper lhs, MembershipUserWrapper rhs)
                             {
                                 if (lhs.Email == null | rhs.Email == null)
                                 {
                                     return 0;
                                 }
                                 else
                                 {
                                     return lhs.Email.CompareTo(rhs.Email);
                                 }
                             }
                             );
                        break;
                    case "Comment":
                        comparison = new Comparison<MembershipUserWrapper>(
                             delegate(MembershipUserWrapper lhs, MembershipUserWrapper rhs)
                             {
                                 if (lhs.Comment == null | rhs.Comment == null)
                                 {
                                     return 0;
                                 }
                                 else
                                 {
                                     return lhs.Comment.CompareTo(rhs.Comment);
                                 }
                             }
                             );
                        break;
                    case "CreationDate":
                        comparison = new Comparison<MembershipUserWrapper>(
                             delegate(MembershipUserWrapper lhs, MembershipUserWrapper rhs)
                             {
                                 return lhs.CreationDate.CompareTo(rhs.CreationDate);
                             }
                             );
                        break;
                    case "IsApproved":
                        comparison = new Comparison<MembershipUserWrapper>(
                             delegate(MembershipUserWrapper lhs, MembershipUserWrapper rhs)
                             {
                                 return lhs.IsApproved.CompareTo(rhs.IsApproved);
                             }
                             );
                        break;
                    case "IsOnline":
                        comparison = new Comparison<MembershipUserWrapper>(
                             delegate(MembershipUserWrapper lhs, MembershipUserWrapper rhs)
                             {
                                 return lhs.IsOnline.CompareTo(rhs.IsOnline);
                             }
                             );
                        break;
                    case "LastLoginDate":
                        comparison = new Comparison<MembershipUserWrapper>(
                             delegate(MembershipUserWrapper lhs, MembershipUserWrapper rhs)
                             {
                                 return lhs.LastLoginDate.CompareTo(rhs.LastLoginDate);
                             }
                             );
                        break;
                    default:
                        comparison = new Comparison<MembershipUserWrapper>(
                             delegate(MembershipUserWrapper lhs, MembershipUserWrapper rhs)
                             {
                                 return lhs.UserName.CompareTo(rhs.UserName);
                             }
                             );
                        break;
                }

                if (sortData.EndsWith("DESC"))
                {
                    memberList.Sort(comparison);
                    memberList.Reverse();
                }
                else
                {
                    memberList.Sort(comparison);
                }

            }

            // FxCopy will give us a warning about returning a List rather than a Collection.
            // We could copy the data, but not worth the trouble.
            return memberList;

        }
    }
}
