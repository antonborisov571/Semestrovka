<!DOCTYPE html>
<html lang="ru-RU">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Недавняя активность</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css"
        crossorigin="anonymous">
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <link rel="stylesheet" href="/styles/background.css" />
    <link rel="shortcut icon" href="/images/tractor.png">
</head>

<body>
    <div id="header_placer" class="header"></div>
    <h2 class="text-center">Недавняя активность</h2>
    {{for activity in Model}}
    <div class="container mt-2">
        <div class="row">
            <div class="card mb-2">
                <div class="card-body">
                    <ul class="list-unstyled js-newsFeedTarget">
                        <li class="media" style="display: flex; align-items: center;">
                            <div class="contentRow-figure" style="margin-right: 15px;">
                                <img src="{{activity.User.UrlImage}}" alt="Avatar" class="avatar rounded"
                                    style="height:72px;width:72px">
                            </div>
                            <div class="media-body">
                                <h5 class="mt-0 mb-1"><a href="/users/{{activity.User.Id}}"
                                        class="username">{{activity.User.Login}}</a></h5>
                                {{if activity.IsLike}}
                                <p>оценил(а) <i class="bi bi-heart-fill" style="color:red"></i> <a
                                        href="/users/{{activity.Sender.Id}}">{{activity.Sender.Login}}</a>-a сообщение в
                                    теме <a href="/topic/{{activity.Post.Id}}">{{activity.Post.PostName}}</a>.</p>
                                <p class="contentRow-snippet">{{if activity.MessageSend.TextMessage.Length >
                                    11}}{{activity.MessageSend.TextMessage.Substring(0, 9)}}...
                                    {{else}}{{activity.MessageSend.TextMessage}}
                                    {{endif}}</p>
                                <p class="contentRow-minor">{{activity.Like.DateDispatch}}</p>
                                {{else}}
                                <p>написал(а) <i class="bi bi-pencil-fill" style="color:blue"></i> сообщение в теме <a
                                        href="/topic/{{activity.Post.Id}}">{{activity.Post.PostName}}</a>.</p>
                                <p class="contentRow-snippet">{{if activity.Message.TextMessage.Length >
                                    11}}{{activity.Message.TextMessage.Substring(0, 9)}}...
                                    {{else}}{{activity.Message.TextMessage}}
                                    {{endif}}</p>
                                <p class="contentRow-minor">{{activity.Message.DateDispatch}}</p>
                                {{endif}}
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
    {{endfor}}
    <script type="text/javascript" src="/js/addHeader.js"></script>
</body>

</html>