﻿namespace TaskManager.Core.ViewModel
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class PermissionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
    }
}
