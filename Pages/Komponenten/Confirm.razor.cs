using Blazored.Modal;
using Blazored.Modal.Services;

using Microsoft.AspNetCore.Components;

namespace BlazorLibrary.Pages.Komponenten
{
	partial class Confirm
	{
		[CascadingParameter] private BlazoredModalInstance BlazoredModal { get; set; }

		private async Task Close() => await BlazoredModal.CloseAsync(ModalResult.Ok(true));
	}
}