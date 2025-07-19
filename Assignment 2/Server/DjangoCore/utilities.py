from django.http import HttpResponse
import json

# default is set to ERROR message
def server_message_response(message, identifier="ERROR", extra_message="server", status=400):
    # ServerMessages are also available as models if one wants to use the messages as data for something
    return HttpResponse(server_message_json(message=message, identifier=identifier, extra_message=extra_message),
                        status=status)


# json representation
def server_message_json(message, identifier="ERROR", extra_message="server"):
    return json.dumps({
        "extraMessage": extra_message,
        "message": message,
        "identifier": identifier
    })

def serialize_card(card):
    return {
        "type": card.type,
        "lvl": card.lvl,
        "count": card.count
    }

def serialize_character(character):
    return {
        "type": character.type,
        "lvl": character.lvl,
        "hp": character.hp,
        "deck": [serialize_card(card) for card in character.deck] if character.deck else []
    }

def serialize_user(user):
    return {
        "uid": str(user.id),
        "name": user.name,
        "lvl": user.lvl,
        "gold": user.gold,
        "upgradePoints": user.upgradePoints,
        "selectedCharacter": user.selectedCharacter,
        "cards": [serialize_card(card) for card in user.cards] if user.cards else [],
        "characters": [serialize_character(c) for c in user.characters] if user.characters else [],
        "friends": user.friends if user.friends else []
    }

def serialize_party(party):
    return {
        "pid": party.pid,
        "hp": party.hp,
        "shield": party.shield,
        "members": party.members if party.members else [],
        "PoIIDs": party.PoIIDs if party.PoIIDs else [],
    }