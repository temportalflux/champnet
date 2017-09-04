/*
	Dustin Yost 0984932
	EGP-405-02
	Lab1
	09/03/2017
	We certify that this work is entirely our own.
	The assessor of this project may reproduce this project and provide
	copies to other academic staff, and/or communicate a copy of this project
	to a plagiarism-checking service, which may retain a copy of
	the project on its database.
*/
#include "network\PacketManager.h"

#include <RakNet\RakNetTypes.h>  // MessageID

namespace Network {

	PacketManager::PacketManager() {
		// instantiate the packet handler map
		this->packetHandlers = Map();
	}

	void PacketManager::registerHandler(PacketID packetID, Value handler) {
		// Add the mapping for some packet ID and some handler
		this->packetHandlers.insert(Pair(packetID, handler));
	}

	PacketManager::Value PacketManager::getHandlerFor(PacketID id) {
		Map::iterator iter;
		// iterate over all handlers
		for (iter = this->packetHandlers.begin(); iter != this->packetHandlers.end(); ++iter) {
			if (iter->first == id) {
				// return the first handler linked to the packet id
				return iter->second;
			}
		}
		// return null if none exists
		return NULL;
	}

}
