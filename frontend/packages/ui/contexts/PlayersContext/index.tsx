import React, { createContext, useContext, useState, ReactNode } from 'react';

interface Player {
    id: string;
    name: string;
}

interface PlayersContextProps {
    players: Player[];
    setPlayers: (players: Player[]) => void;
}

const PlayersContext = createContext<PlayersContextProps | undefined>(undefined);

export const PlayersProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [players, setPlayers] = useState<Player[]>([]);


    return (
        <PlayersContext.Provider value={{ players, setPlayers }}>
            {children}
        </PlayersContext.Provider>
    );
};

export const usePlayers = (): PlayersContextProps => {
    const context = useContext(PlayersContext);
    if (!context) {
        throw new Error('usePlayers must be used within a PlayersProvider');
    }
    return context;
};