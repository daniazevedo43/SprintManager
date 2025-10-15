"use client"

import React, { useState } from 'react';
import Form from 'next/form'
import { RegisterRequest } from '../../types/auth';

export default function Register() {

    const [name, setName] = useState("");
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");

    const handleSubmit = async (e: React.FormEvent) => {

        e.preventDefault();

        const request: RegisterRequest = {
            name: name,
            username: username,
            email: email,
            password: password
        }

        await fetch('https://localhost:7060/api/Auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(request),
        });
    };

    return (
        <div className='flex justify-center items-center min-h-screen'>
            <Form 
                className='flex flex-col w-125' 
                action=""
                onSubmit={handleSubmit}
            >
                <label>Name:</label>
                <input 
                    className='border black' 
                    name="name" 
                    onChange={(event) => setName(event.target.value)}
                />
                
                <label>Username:</label>
                <input 
                    className='border black' 
                    name="username" 
                    onChange={(event) => setUsername(event.target.value)}
                />

                <label>Email:</label>
                <input 
                    className='border black' 
                    name="email"
                    onChange={(event) => setEmail(event.target.value)}
                />

                <label>Password:</label>
                <input 
                    className='border black' 
                    name="password" 
                    type='password'
                    onChange={(event) => setPassword(event.target.value)}
                />
                
                <button type="submit">Submit</button>
            </Form>
        </div>
    )
}