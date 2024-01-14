<!DOCTYPE html>
<html>

<head>
  <meta charset="utf-8">
  <title>Новые сообщения</title>
  <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/css/bootstrap.min.css" rel="stylesheet"
    integrity="sha384-4bw+/aepP/YC94hEpVNVgiZdgIC5+VKNBQNGCHeKRQN+PtmoHDEXuppvnDJzQIu9" crossorigin="anonymous">
  <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.1/dist/js/bootstrap.bundle.min.js"
    integrity="sha384-HwwvtgBNo3bZJJLYd8oVXjrBZt8cqVSpeBNS5n7C8IVInixGAoxmnlMuBnhbgrkm"
    crossorigin="anonymous"></script>
  <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
  <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css"
    crossorigin="anonymous">
  <link rel="stylesheet" href="/styles/background.css" />
  <link rel="stylesheet" href="/styles/star.css" />
  <link rel="stylesheet" href="/styles/heart.css" />
  <link rel="stylesheet" href="/styles/username.css" />
  <link rel="shortcut icon" href="/images/tractor.png">
</head>

<body>
  <div id="header_placer" class="header"></div>
  <div class="container">
    <div class="container mt-4">
      <div class="row">
        <div class="col-12">
          <h2>Новые сообщения </h2>
          {{for message in Model.Messages}}
          <div class="container mt-5">
            <div class="row">
              <div class="col-md-3">
                <div class="card mt-4">
                  <div class="card-body" id="message_userinfo_{{message.MessageInfo.Id}}">
                    <img class="rounded" src="{{message.User.UrlImage}}" alt="Фото пользователя"
                      style="height:120px;width:120px">
                    <a class="user-name" href="/users/{{message.User.Id}}">
                      <h5 class="card-title">{{message.User.Login}}</h5>
                    </a>
                    <p class="card-text">Трактор: {{message.User.TractorName}}</p>
                    <p class="card-text">Репутация: {{message.User.Reputation}}</p>
                    <p class="card-text">С нами с: {{message.User.DateRegistration.ToShortDateString()}}</p>
                    {{if message.IsOnline}}
                    <p class="card-text">Последняя активность: В сети <i class="bi bi-circle-fill"
                        style="color:green"></i></p>
                    {{else}}
                    <p class="card-text">Последняя активность: {{message.User.LastOnline.ToString("g")}}</p>
                    {{endif}}
                  </div>
                </div>
              </div>
              <div class="col-md-9">
                <div class="mt-4">
                  <div class="card mb-2">
                    <div class="card-body">
                      <div class="d-flex justify-content-between">
                        <div>
                          <small class="text-muted"
                            id="message_time_{{message.MessageInfo.Id}}">{{message.MessageInfo.DateDispatch}} / <a
                              href="/topic/{{message.PostInfo.TopicId}}/messages/{{message.PostInfo.Id}}">{{message.PostInfo.PostName}}</a></small>
                        </div>
                        {{if Model.SessionInfo != ""}}
                        <div>
                          <span id="star_{{message.MessageInfo.Id}}" class="star" data-toggle="tooltip"
                            data-placement="top" title="Добавить в избранное" onclick="addFavourite(this)"
                            messageid="{{message.MessageInfo.Id}}" hasfavourite="{{message.HasFavourite}}" {{if
                            message.HasFavourite}} style="color:orange;" {{else}} style="color:gray;" {{endif}}>
                            <i class="bi bi-star"></i>
                          </span>
                          <span id="heart_{{message.MessageInfo.Id}}" class="heart" data-toggle="tooltip"
                            data-placement="top" title="Поставить лайк" onclick="setLike(this)"
                            messageid="{{message.MessageInfo.Id}}" haslike="{{message.HasLike}}" {{if message.HasLike}}
                            style="color:red;" {{else}} style="color:gray;" {{endif}}>
                            <i class="bi bi-heart"></i>
                          </span>
                        </div>
                        {{else}}{{endif}}
                      </div>
                      <p class="mb-1 w-100" id="message_desc_{{message.MessageInfo.Id}}">
                        {{message.MessageInfo.TextMessage}}</p>
                    </div>
                  </div>
                </div>
              </div>
              {{endfor}}
            </div>
            <div class="col-md-4">
            </div>
          </div>
        </div>
      </div>
</body>
<script type="text/javascript" src="/js/addHeader.js"></script>
<script type="text/javascript" src="/js/addPost.js"></script>

</html>