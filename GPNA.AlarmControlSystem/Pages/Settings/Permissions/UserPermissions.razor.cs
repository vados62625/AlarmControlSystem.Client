using Blazored.Toast.Services;
using GPNA.AlarmControlSystem.Extensions;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Models.Dto.User;
using GPNA.AlarmControlSystem.Models.Enums;
using Microsoft.AspNetCore.Components;
using AccessType = GPNA.AlarmControlSystem.Domain.Enums.AccessType;

namespace GPNA.AlarmControlSystem.Pages.Settings.Permissions;

public partial class UserPermissions : AcsPageBase
{
    private bool _editMode;

    private List<UserDto> Admins { get; set; } = new();
    private List<UserDto> AdminModels { get; set; } = new();
    private List<UserDto> Users { get; set; } = new();
    private List<UserDto> UserModels { get; set; } = new();

    [Inject] private IToastService ToastService { get; set; } = null!;
    [Inject] private IUserService UserService { get; set; } = null!;


    protected override async Task LoadPageAsync()
    {
        await SpinnerService.Load(GetUsers);
    }

    private async Task GetUsers()
    {
        var result = await UserService.GetList();

        if (!result.Success) return;

        var users = result.Payload
            .GroupBy(c => c.Access is AccessType.Admin or AccessType.SuperAdmin)
            .ToHashSet();

        Admins = users
            .Where(c => c.Key)
            .SelectMany(c => c)
            .ToList();

        Users = users
            .Where(c => !c.Key)
            .SelectMany(c => c)
            .ToList();

        AdminModels = Admins.Select(c => c.Copy()).ToList();

        UserModels = Users.Select(c => c.Copy()).ToList();
    }

    private void OnCancelClick()
    {
        _editMode = false;

        AdminModels = Admins.Select(c => c.Copy()).ToList();

        UserModels = Users.Select(c => c.Copy()).ToList();

        StateHasChanged();
    }

    private async Task OnSaveClick()
    {
        var usersToCreateOrUpdate = UserModels.Where(userModel => {
            var user = Users.FirstOrDefault(u => u.Id == userModel.Id);
            return !userModel.Equals(user);
        }).ToList();

        await CreateOrUpdateUsers(usersToCreateOrUpdate);

        _editMode = false;
    }

    private async Task CreateOrUpdateUsers(IEnumerable<UserDto> models)
    {
        using var adminsEnumerator = models.GetEnumerator();
        while (adminsEnumerator.MoveNext())
        {
            var model = adminsEnumerator.Current;
            switch (model.Id == default)
            {
                case true:
                {
                    await UserService.Create(model);
                    break;
                }
                case false:
                {
                    var result =
                        await UserService.Update(new UpdateUserCommand { Id = model.Id, Access = model.Access });
                    if (result.Success)
                        model = result.Payload;
                    break;
                }
            }
        }
    }

    private void ShowSuccess() => ToastService.ShowSuccess("Данные сохранены");
}