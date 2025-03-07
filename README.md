# Reichan API

Reichan API is an ASP.NET-based API inspired by 4chan. It provides a platform for users to interact with discussion boards and manage posts programmatically.

## Documentation

The API documentation is available at the following link:
[Reichan API Docs](https://reichan-api.up.railway.app/docs/v1)

## Features
- Create, read,vote posts.

## Installation
1. Clone the repository:
   ```sh
   git clone https://github.com/Alekssandher/reichan-api.git
   ```
2. Navigate to the project directory:
   ```sh
   cd reichan-api
   ```
3. Install dependencies:
   ```sh
   dotnet restore
   ```
4. Configure the environment variables for database and API settings:
   ```
   DATABASE_URL = ""
   DATABASE_NAME = ""
   
   POSTS_COLLECTION = ""
   USERS_COLLECTION = ""
   REPLIES_COLLECTION = ""
   
   # true or false
   ENABLE_SWAGGER = ""
   
   #CLOUDINARY
   CLOUDINARY_NAME = ""
   CLOUDINARY_KEY = ""
   CLOUDINARY_SECRET = ""
   ```
6. Run the application:
   ```sh
   dotnet run
   ```
   
## License
This project is licensed under the GPL-3 License.

## Contributing
Feel free to submit pull requests or open issues to improve Reichan API.

## Contact
For inquiries or support, reach out via the repository's issue tracker.

