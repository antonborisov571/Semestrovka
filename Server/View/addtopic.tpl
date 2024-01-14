<!DOCTYPE html>
<html>

<head>
  <meta charset="utf-8">
  <title>Добавление темы</title>
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
  <form method="post" action="addtopic">
    <div class="container mt-4">
      <div class="container mt-4">
        <div class="row">
          <div class="col-md-8">
            <h2>Добавить новую тему</h2>
            <form>
              <div class="form-group">
                <label for="topicName">Название темы</label>
                <input type="text" class="form-control" name="topicName" id="topicName"
                  placeholder="Введите название темы">
              </div>
              <div class="form-group">
                <label for="topicDescription">Описание темы</label>
                <textarea class="form-control" name="topicDescription" id="topicDescription" rows="4"
                  placeholder="Введите описание темы"></textarea>
              </div>
              <br><br>
              <button type="submit" class="btn btn-primary">Добавить тему</button>
            </form>
          </div>
        </div>
      </div>
    </div>
</body>
<script type="text/javascript" src="/js/addHeader.js"></script>

</html>