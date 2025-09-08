using CommunityToolkit.Maui.Core;

namespace Daily.Toasts
{
    public class DiaryRecordToastHandler
    {
        private static readonly IToast _diaryRecordCreatedToast = ToastCreator.CreateShortByTime(diaryRecordCreatedMessage);
        private static readonly IToast _diaryRecordEditedToast = ToastCreator.CreateShortByTime(diaryRecordEditedMessgae);
        private static readonly IToast _diaryRecordDeletedToast = ToastCreator.CreateShortByTime(diaryRecordDeletedMessage);

        private static readonly IToast _diaryRecordErrorToast = ToastCreator.CreateLongByTime(diaryRecordErrorMessage);

        private const string diaryRecordCreatedMessage = "Запись была успешно создана";
        private const string diaryRecordEditedMessgae = "Запись была успешно изменена";
        private const string diaryRecordDeletedMessage = "Запись удалена";

        private const string diaryRecordErrorMessage = "Ошибка\nПопробуйте еще раз";

        public static async Task ShowDiaryRecordCreatedToastAsync() => await _diaryRecordCreatedToast.Show();

        public static async Task ShowDiaryRecordEditedToastAsync() => await _diaryRecordEditedToast.Show();

        public static async Task ShowDiaryRecordDeletedToastAsync() => await _diaryRecordDeletedToast.Show();

        public static async Task ShowDiaryRecordErrorToastAsync() => await _diaryRecordErrorToast.Show();
    }
}