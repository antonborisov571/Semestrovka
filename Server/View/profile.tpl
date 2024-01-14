<!DOCTYPE html>
<html>

<head>
  <meta charset="utf-8">
  <title>Профиль {{Model.Login}}</title>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/css/bootstrap.min.css" rel="stylesheet"
    integrity="sha384-4bw+/aepP/YC94hEpVNVgiZdgIC5+VKNBQNGCHeKRQN+PtmoHDEXuppvnDJzQIu9" crossorigin="anonymous">
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/js/bootstrap.bundle.min.js"
    integrity="sha384-HwwvtgBNo3bZJJLYd8oVXjrBZt8cqVSpeBNS5n7C8IVInixGAoxmnlMuBnhbgrkm"
    crossorigin="anonymous"></script>
  <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
  <link rel="stylesheet" href="/styles/background.css" />
  <link rel="shortcut icon" href="/images/tractor.png">
</head>

<body>
  <div id="header_placer" class="header"></div>

  <div class="container">
    <div class="main_content text-center">
      <h2>Информация о пользователе</h2>
      <div class="row">
        <div class="col-lg-4">
          <div class="card mb-4">
            <div class="card-body text-center">
              <img src="{{Model.UrlImage}}" alt="avatar" class="rounded-circle img-fluid" style="width: 150px;">
              <h5 class="my-3">{{Model.Login}}</h5>
            </div>
          </div>
        </div>
        <div class="col-lg-8">
          <div class="card mb-4">
            <div class="card-body">
              <div class="row">
                <div class="col-sm-3">
                  <p class="mb-0">Имя</p>
                </div>
                <div class="col-sm-9">
                  <p class="text-muted mb-0">{{Model.Login}}</p>
                </div>
              </div>
              <hr>
              <div class="row">
                <div class="col-sm-3">
                  <p class="mb-0">Email</p>
                </div>
                <div class="col-sm-9">
                  <p class="text-muted mb-0">{{Model.Email}}</p>
                </div>
              </div>
              <hr>
              <div class="row">
                <div class="col-sm-3">
                  <p class="mb-0">Трактор</p>
                </div>
                <div class="col-sm-9">
                  <p class="text-muted mb-0">{{Model.TractorName}}</p>
                </div>
              </div>
              <hr>
              <div class="row">
                <div class="col-sm-3">
                  <p class="mb-0">Репутация</p>
                </div>
                <div class="col-sm-9">
                  <p class="text-muted mb-0">{{Model.Reputation}}</p>
                </div>
              </div>
              <hr>
              <div class="row">
                <div class="col-sm-3">
                  <p class="mb-0">Дата регистрации</p>
                </div>
                <div class="col-sm-9">
                  <p class="text-muted mb-0">{{Model.DateRegistration.ToShortDateString()}}</p>
                </div>
              </div>
              <hr>
              <div class="row">
                <div class="col-sm-3">
                  <p class="mb-0">Последний раз был в сети</p>
                </div>
                <div class="col-sm-9">
                  <p class="text-muted mb-0">{{Model.LastOnline.ToString("g")}}</p>
                </div>
              </div>
              <hr>
              <div class="row">
                <form class="col-sm-7" id="uploadForm" enctype="multipart/form-data">
                  <p class="mb-0">Загрузи cвоё фото</p>
                  <br>
                  <input type='file' id="formFileLg" name="image">
                  <br><br>
                  <button id="submitButton" type="button" class="btn btn-primary"
                    accept=".jpg, .jpeg, .png">Отправить</button>
                </form>
              </div>
            </div>
          </div>
          <form>
            <button type="submit" formaction="/profile" formmethod="post" class="btn btn-danger">Выход</button>
          </form>
        </div>
      </div>

      <script type="text/javascript" src="/js/uploadImageProfile.js"></script>
      <script type="text/javascript" src="/js/addHeader.js"></script>
</body>

</html>