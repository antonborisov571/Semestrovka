<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css"
  crossorigin="anonymous">
<div>
  <nav class="navbar navbar-expand-lg border-bottom border-body" data-bs-theme="dark" style="background-color: #185886">
    <div class="container-fluid">
      <img src="/images/tractor.png" style="height: 50px; margin: 3px">
      <a class="navbar-brand" href="/" style="color:#bcdef5">Фермерские тракторы</a>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarScroll"
        aria-controls="navbarScroll" aria-expanded="false" aria-label="Переключатель навигации">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarScroll">
        <ul class="navbar-nav me-auto my-2 my-lg-0 navbar-nav-scroll" style="--bs-scroll-height: 100px;">
          <li class="nav-item">
            <a class="nav-link" href="/">Главная</a>
          </li>
          <li class="nav-item dropdown">
            <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
              Что нового
            </a>
            <ul class="dropdown-menu" style="background-color: #185886">
              <li><a class="dropdown-item" href="/newmessages">Новые сообщения</a></li>
              <li>
                <hr class="dropdown-divider">
              </li>
              <li><a class="dropdown-item" href="/newposts">Новые посты</a></li>
              <li>
                <hr class="dropdown-divider">
              </li>
            </ul>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="/recentactivity">Недавние действия</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="/topusers">Крутые пользователи</a>
          </li>
          {{if Model != ""}}
          <li class="nav-item">
            <a class="nav-link" href="/favourites">Избранное</a>
          </li>
          {{else}}{{endif}}
        </ul>
        {{if Model == ""}}
        <ul class="nav navbar-nav navbar-right">
          <li class="nav-item">
            <a class="nav-link" href="/login">Вход</a>
          </li>
          <li class="nav-item">
            <a class="nav-link" href="/register">Регистрация</a>
          </li>
        </ul>
        {{else}}
        <ul class="nav navbar-nav navbar-right">
          <li class="nav-item">
            <a class="nav-link" href="/profile">Профиль</a>
          </li>
        </ul>
        {{endif}}
        <form class="d-flex" role="search" style="max-width: 400px;">
          <div class="dropdown me-2">
            <button class="btn btn-secondary dropdown-toggle" type="button" id="typeFilterDropdown"
              data-bs-toggle="dropdown">
              <i class="bi bi-filter"></i>
            </button>
            <ul class="dropdown-menu" aria-labelledby="typeFilterDropdown">
              <li><a class="dropdown-item active" id="data_type_all" data-type="all">Все <i class="bi bi-globe"></i></a>
              </li>
              <li><a class="dropdown-item" id="data_type_topics" data-type="topics">Темы <i class="bi bi-book"></i></a>
              </li>
              <li><a class="dropdown-item" id="data_type_posts" data-type="posts">Посты <i
                    class="bi bi-file-alt"></i></a></li>
              <li><a class="dropdown-item" id="data_type_messages" data-type="messages">Сообщения <i
                    class="bi bi-comment"></i></a></li>
              <div class="dropdown-divider"></div>
              <li><a class="dropdown-item active" id="data_time_all" data-time="all">Все время <i
                    class="bi bi-infinity"></i></a></li>
              <li><a class="dropdown-item" id="data_time_day" data-time="day">За день <i class="bi bi-sun"></i></a></li>
              <li><a class="dropdown-item" id="data_time_week" data-time="week">За неделю <i
                    class="bi bi-calendar-week"></i></a></li>
              <li><a class="dropdown-item" id="data_time_month" data-time="month">За месяц <i
                    class="bi bi-calendar-alt"></i></a></li>
            </ul>
          </div>
          <input id="textFind" class="form-control me-2 bg-light" type="search" placeholder="Поиск" aria-label="Поиск"
            style="color: black; flex-grow: 1; max-width:200px" />

          <button id="submitButton" class="btn btn-success" type="button">Поиск</button>
        </form>

      </div>
    </div>
  </nav>
</div>
<script type="text/javascript" src="/js/selectedFilters.js"></script>