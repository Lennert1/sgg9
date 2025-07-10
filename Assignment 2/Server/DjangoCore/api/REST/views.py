from django.http import JsonResponse
from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from rest_framework.response import Response
from ServerClass import ServerClass
import json
import utilities

# Create your views here.
@csrf_exempt
def change_color(request):
    color_data = json.loads(request.body).get("color")
    ServerClass.color = int(color_data)
    print("Empfangene Farbe (REST):", color_data)
    return  utilities.server_message_response(f"color: {color_data}","DATA")

@csrf_exempt
def login(request):
    name = json.loads(request.body).get("name")
    uid = json.loads(request.body).get("uid")

    print("Name: ", name)
    print("UID: ", uid)
    return utilities.server_message_response("received","STATUS")

@csrf_exempt
def update_color(request):
    msg = json.dumps({
        "color": f"{ServerClass.color}"
    })
    return utilities.server_message_response(msg,"DATA")

# =============================== Stuff that Han did ========================================

# Receives input from Login UI and checks if user exists
# If yes a json is sent to the frontend
@csrf_exempt
def check_login(request):
    if request.method != "POST":
        return utilities.server_message_response("Not a POST request was sent", "405", status = 405)

    # Checking the login based on the username
    try:
        data = json.loads(request.body)
        username = data.get('name')
    except json.JSONDecodeError:
        return utilities.server_message_response("Invalid Json!", "400", status = 400)

    # Class method. Searches all the instances and returns None if user does not exist
    user = ServerClass.get_user_by_name(username)

    if user is None:
        return utilities.server_message_response("User was not found", "404", status=404)
    else:
        user_data = {
            "name": user.username,
            "uid": user.uid,
            "lvl": user.level,
            "gold": user.gold,
            "armorPoints": user.armorPoints
        }
        # Sends a Json Response back to the frontend
        return JsonResponse(user_data, status=200)

# This method is not finished yet
# This should add an instance to the ServerClass
@csrf_exempt
def register(request):
    if request.method == "POST":
        try:
            data = json.loads(request.body)
            print("Request body raw:", request.body)
            name = data.get("name")

            # The UID should be assigned by the server I think
            # uid = data.get("uid")

            print(f"Name: {name}")

            return utilities.server_message_response("received","STATUS", status=200)
        except Exception as e:
            return utilities.server_message_response("received","STATUS", status=400)
    else:
        return utilities.server_message_response("received","STATUS", status=405)




