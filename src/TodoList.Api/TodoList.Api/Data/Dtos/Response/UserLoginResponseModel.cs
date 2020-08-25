namespace TodoList.Api.Data.Dtos.Response {
    public class UserLoginResponseModel {
        public bool Succeeded { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}