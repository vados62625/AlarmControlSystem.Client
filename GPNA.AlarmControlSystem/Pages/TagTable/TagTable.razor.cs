using Blazored.Modal;
using Blazored.Modal.Services;
using GPNA.AlarmControlSystem.Interfaces;
using GPNA.AlarmControlSystem.Models.Dto;
using GPNA.AlarmControlSystem.Pages.TagTable.Modals;
using GPNA.AlarmControlSystem.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace GPNA.AlarmControlSystem.Pages.TagTable
{
    public partial class TagTable : ComponentBase
    {
        [Inject]
        protected ISpinnerService SpinnerService { get; set; } = default!;
        [Inject]
        protected ITagService TagService { get; set; } = default!;

        [CascadingParameter]
        public IModalService Modal { get; set; } = default!;

        public List<TagDto>? AlarmsTagTable;

        string input = "";


        protected override async Task OnInitializedAsync() => await GetTags();

        public async Task AddTag()
        {
            var options = new ModalOptions();
            options.Class = "modal-2";
            var createModal = Modal.Show<AddTagModal>("Добавить Tag", options);
            var result = await createModal.Result;

            if (result.Confirmed)
            {
                SpinnerService.Show();
                await GetTags();
                SpinnerService.Hide();
            }
        }

        public async Task EditTag(TagDto tag)
        {
            var options = new ModalOptions();
            options.Class = "modal-2";
            var parameters = new ModalParameters();
            parameters.Add("Tag", tag);

            var createModal = Modal.Show<EditTagModal>("Изменить Tag", parameters, options);
            var result = await createModal.Result;

            if (result.Confirmed)
            {
                SpinnerService.Show();
                await GetTags();
                SpinnerService.Hide();
            }
        }

        public async Task DeleteTag(TagDto tag)
        {
            ArgumentNullException.ThrowIfNull(tag.Id);
            var parameters = new ModalParameters();
            parameters.Add("TagName", tag.TagName);
            var options = new ModalOptions();
            options.Class = "modal-delete";
            var deleteModal = Modal.Show<DeleteTagModal>("Удаление Tag", parameters, options);
            var result = await deleteModal.Result;

            if (result.Confirmed)
            {
                SpinnerService.Show();
                await TagService.Delete(tag.Id);
                await GetTags();
                SpinnerService.Hide();
            }
        }

        private async Task Enter(KeyboardEventArgs e)
        {
            if (e.Code == "Enter" || e.Code == "NumpadEnter")
            {
                await GetTags();
            }
        }

        private async Task GetTags()
        {
            SpinnerService.Show();

            var tags = await TagService.GetList(); // TODO I make PageableCollectionDto
            var alarmsTagTable = new List<TagDto>(tags.Payload); //TODO need check success

            if (input != "")
            {
                AlarmsTagTable = new();
                foreach (var context in alarmsTagTable)
                {
                    if (context != null && context.TagName != null)
                    {
                        if (context.TagName.ToUpper().IndexOf(input.ToUpper()) > -1)
                        {
                            AlarmsTagTable.Add(context);
                        }
                    }
                }
            }
            else
            {
                AlarmsTagTable = alarmsTagTable;
            }
            SpinnerService.Hide();
        }
    }
}
