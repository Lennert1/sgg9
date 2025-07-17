from django.http import JsonResponse
from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from rest_framework.response import Response
from ServerClass import ServerClass, Parties
import json
import utilities
from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi


uri = "mongodb+srv://hannguyen30052003:8YYgCLne0GkLrTsx@gamedatabase.mro6jbt.mongodb.net/?retryWrites=true&w=majority&appName=GameDatabase"
# Create a new client and connect to the server
client = MongoClient(uri, server_api=ServerApi('1'))
# Send a ping to confirm a successful connection
try:
    client.admin.command('ping')
    print("Pinged your deployment. You successfully connected to MongoDB!")
except Exception as e:
    print(e)


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
            "armorPoints": user.armorPoints,
            "cards": [{"type": card[0], "lvl": card[1], "count": card[2]} for card in user.listOfCards]
        }
        # Sends a Json Response back to the frontend
        return JsonResponse(user_data, status=200)

# This method is not finished yet
# This should add an instance to the ServerClass/ database
@csrf_exempt
def register(request):
    if request.method == "POST":
        try:
            data = json.loads(request.body)
            print("Request body raw:", request.body)
            name = data.get("name")
            print(f"Name: {name}")

            # The UID should be assigned by the server I think
            # uid = data.get("uid")

            # Here should be the call to send data to the data base

            return utilities.server_message_response("received","STATUS", status=200)
        except Exception as e:
            return utilities.server_message_response("received","STATUS", status=400)
    else:
        return utilities.server_message_response("received","STATUS", status=405)

@csrf_exempt
def updateData(request):
    if request.method == "POST":
        try:
            data = json.loads(request.body)
            print(data)
            gold = data.get("updatedGold")
            cards = data.get("updatedCards")

            # Send the updated data to the database

            return utilities.server_message_response("received", "STATUS", status=200)
        except Exception as e:
            return utilities.server_message_response("received", "STATUS", status=400)
    else:
        return utilities.server_message_response("received", "STATUS", status=405)

# Searches in the database for the id and returns the data in a json
@csrf_exempt
def userById(request, uid):
    print(f"Benutzer-ID: {uid}")
    if request.method == "GET":
        # Search for user
        user = ServerClass.get_user_by_id(uid)
        if user is not None:
            user_data = {
                "name": user.username,
                "uid": user.uid,
                "lvl": user.level,
                "gold": user.gold,
                "armorPoints": user.armorPoints,
                "cards": [{"type": card[0], "lvl": card[1], "count": card[2]} for card in user.listOfCards]
            }
            return JsonResponse(user_data)
        else:
            return JsonResponse({'error': 'User not found'}, status=404)
    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)


@csrf_exempt
def partyById(request, pid):
    if request.method == "POST":
        # Search for user
        party = Parties.get_party_by_pid(pid)
        if party is not None:
            party_data = {
                "pid": party.pid,
                "members": [ member for member in party.members],
                # I dunno where this is even used
                "memberPoIids": []
            }
            return JsonResponse(party_data)
        else:
            return JsonResponse({'error': 'Party not found'}, status=404)
    else:
        return JsonResponse({'error': 'Invalid request method'}, status=400)




