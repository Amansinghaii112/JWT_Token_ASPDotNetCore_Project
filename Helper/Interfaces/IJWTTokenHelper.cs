namespace JWT_Token_Project
{
    public interface IJWTTokenHelper
    {

        string JWTTokenGenerator(UserDTO user);

    }
}
