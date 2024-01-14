<!DOCTYPE html>
<html>

<head>
  <meta charset="utf-8">
  <title>Топ пользователей</title>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/css/bootstrap.min.css" rel="stylesheet"
    integrity="sha384-4bw+/aepP/YC94hEpVNVgiZdgIC5+VKNBQNGCHeKRQN+PtmoHDEXuppvnDJzQIu9" crossorigin="anonymous">
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/js/bootstrap.bundle.min.js"
    integrity="sha384-HwwvtgBNo3bZJJLYd8oVXjrBZt8cqVSpeBNS5n7C8IVInixGAoxmnlMuBnhbgrkm"
    crossorigin="anonymous"></script>
  <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
  <link rel="stylesheet" href="/styles/background.css" />
  <link rel="stylesheet" href="/styles/username.css" />
  <link rel="shortcut icon" href="/images/tractor.png">
</head>

<body>
  <div id="header_placer" class="header"></div>
  {{count = 1}}
  <h2 class="text-center">Топ 100 самых авторитетных пользователей</h2>
  {{for model in Model}}
  <div class="container">
    <div class="main_content text-center">
      <h2>{{count}} место {{count = count + 1}}</h2>
      <div class="row">
        <div class="col-lg-4">
          <div class="card mb-4">
            <div class="card-body text-center">
              <img src="{{model.UrlImage}}" alt="avatar" class="rounded-circle img-fluid" style="width: 150px;">
              <a class="user-name" href="/users/{{model.Id}}">
                <h5 class="my-3">{{model.Login}}</h5>
              </a>
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
                  <p class="text-muted mb-0">{{model.Login}}</p>
                </div>
              </div>
              <hr>
              <div class="row">
                <div class="col-sm-3">
                  <p class="mb-0">Email</p>
                </div>
                <div class="col-sm-9">
                  <p class="text-muted mb-0">{{model.Email}}</p>
                </div>
              </div>
              <hr>
              <div class="row">
                <div class="col-sm-3">
                  <p class="mb-0">Трактор</p>
                </div>
                <div class="col-sm-9">
                  <p class="text-muted mb-0">{{model.TractorName}}</p>
                </div>
              </div>
              <hr>
              <div class="row">
                <div class="col-sm-3">
                  <p class="mb-0">Репутация</p>
                </div>
                <div class="col-sm-9">
                  <p class="text-muted mb-0">{{model.Reputation}}</p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      {{endfor}}
      <script type="text/javascript" src="/js/addHeader.js"></script>
</body>

</html>