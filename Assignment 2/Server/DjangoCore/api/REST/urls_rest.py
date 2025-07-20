from django.urls import path, include
from api.REST import views

urlpatterns = [
    path("change_color/", views.change_color, name="change_color"),
    path("login/", views.login, name="login"),
    path("update_color/", views.update_color, name="update_color"),

    # =========== The stuff that Han did ============================
    path("register/", views.register, name="register"),
    path("check_login/", views.check_login, name="check_login"),
    path("updateData/", views.updateData, name="updateData"),
    path("userById/", views.userById, name="userById"),
    path("partyById/", views.partyById, name = "partyById"),

    path("allParties/", views.allParties, name="allParties"),
    path("joinParty/", views.joinParty, name="joinParty"),
    path("leaveParty/", views.leaveParty, name="leaveParty"),
    path("createParty/", views.createParty, name="createParty"),
    path("getBattleArena/", views.getBattleArena, name="getBattleArena"),
    path("updateBattleArena/", views.updateBattleArena, name="updateBattleArena"),
    path("createBattleArena/", views.createBattleArena, name="createBattleArena"),
]
