using Data.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Tests
{
    internal class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals(User? x, User? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return (x.Email == y.Email && x.UsedDiskSpade == y.UsedDiskSpade
                                                              && x.Id == y.Id
                                                              && x.RoleId == y.RoleId
                                                              && x.Role == y.Role
                                                              && x.Password == y.Password
                                                              && x.DiskSpaceId == y.DiskSpaceId
                                                              && x.DiskSpace == y.DiskSpace);

        }
        public bool Equals(List<User>? x, List<User>? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            for (int i = 0; i < x.Count; i++)
            {
                if (x[i].Email == y[i].Email && x[i].UsedDiskSpade == y[i].UsedDiskSpade
                                                              && x[i].Id == y[i].Id
                                                              && x[i].RoleId == y[i].RoleId
                                                              && x[i].Role == y[i].Role
                                                              && x[i].Password == y[i].Password
                                                              && x[i].DiskSpaceId == y[i].DiskSpaceId
                                                              && x[i].DiskSpace == y[i].DiskSpace && i == x.Count - 1)
                {
                    return true;
                }
            }
            return false;

        }
        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.GetHashCode();
        }

        
    }


    internal class FileEqualityComparer : IEqualityComparer<File>
    {
        public bool Equals(File? x, File? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return (x.Id == y.Id && x.Name == y.Name
                                                              && x.Type == y.Type
                                                              && x.AccessLink == y.AccessLink
                                                              && x.Size == y.Size
                                                              && x.UserId == y.UserId
                                                              && x.ParentId == y.ParentId
                                                              && x.Path == y.Path);

        }
        public bool Equals(List<File>? x, List<File>? y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            for (int i = 0; i < x.Count; i++)
            {
                if (x[i].Id == y[i].Id && x[i].Name == y[i].Name
                                                              && x[i].Type == y[i].Type
                                                              && x[i].AccessLink == y[i].AccessLink
                                                              && x[i].Size == y[i].Size
                                                              && x[i].UserId == y[i].UserId
                                                              && x[i].ParentId == y[i].ParentId
                                                              && x[i].Path == y[i].Path)
                {
                    return true;
                }
            }
            return false;

        }
        public int GetHashCode([DisallowNull] File obj)
        {
            return obj.GetHashCode();
        }


    }
}
